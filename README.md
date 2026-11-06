# MyAppBenchAssignment

A .NET 10 Razor Pages employee management application using EF Core and PostgreSQL. It supports employee create, read, update, and delete operations and exposes `/healthz` for health checks.

## Run locally

Prerequisites: .NET 10 SDK and a reachable PostgreSQL database. The application reads its connection string from `ConnectionStrings__DefaultConnection`; `appsettings.json` intentionally contains no credentials.

In PowerShell, set a connection string for your local PostgreSQL instance, then run the app from the project directory:

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=MyAppBenchAssignment;Username=<user>;Password=<password>"
dotnet restore
dotnet run
```

The app applies EF Core migrations at startup. Development-only sample employees are added only when the employee table is empty. Do not put database credentials in source control.

## AWS deployment

The container image is built by the GitHub Actions workflow and pushed to ECR:

`653011867539.dkr.ecr.ap-southeast-2.amazonaws.com/benchassignment/myapp`

The ECS Fargate service runs the container on port `8080`. Its database connection string is supplied from AWS Secrets Manager; it is not included in the image or repository.

### Request and database traffic

```text
Browser -> internet-facing ALB (HTTP 80)
		-> HTTP listener and rule
		-> IP target group (port 8080)
		-> ECS task private ENI address and application container
```

ECS registers task IP addresses with the target group, which health-checks the targets and routes requests to healthy tasks. The ALB security group permits inbound HTTP; the ECS task security group permits port `8080` from the ALB security group.

The RDS PostgreSQL instance is in private subnets and is not publicly accessible. ECS tasks connect to its private endpoint on port `5432` over the VPC local route. `rds-private-sg` allows that port only from the ECS task security group. This database traffic does not pass through the Internet Gateway.

The ECS tasks currently use public subnets with public IP assignment because no NAT Gateway was created. The Internet Gateway provides internet routing for those public subnets; it is not used for ECS-to-RDS traffic.

### CloudFormation

The CloudFormation stack manages the RDS instance, ECR repository, ECS cluster, task definition, and ECS service. The template exposes `DatabaseInstanceClass` (default `db.t3.micro`) and `TaskSize` (default `Small`, 0.25 vCPU and 0.5 GiB memory). The stack is deployed in `ap-southeast-2`; CloudFormation Region is selected for the stack, and `AWS::Region` is used where a regional value is required. The VPC, subnets, security groups, ALB, listener, and target group remain external dependencies.

Review CloudFormation change sets before executing updates. RDS automated backups are enabled with a one-day retention period.

## Verification

- Open the ALB DNS name in a browser to reach the application.
- Open `/healthz`; it should return HTTP 200 with `{"status":"ok"}`.
- Visit `/Employees` and test employee CRUD operations.
- In ECS, verify the service task is running and the target group reports it healthy.

## Security notes

- Keep database passwords and AWS access keys out of the repository, Docker image, and screenshots.
- The RDS security group should accept PostgreSQL only from the ECS task security group.
- GitHub Actions credentials are stored as repository secrets; rotate or disable them when no longer needed.
