# ECS Cluster
resource "aws_ecs_cluster" "main" {
  name = "drive-dev"
  tags = local.tags
}

# Task Definition
resource "aws_ecs_task_definition" "drive_api" {
  family                   = "drive-dev-api"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_role.arn

  container_definitions = jsonencode([
    {
      name  = "drive-api"
      image = "${local.account_id}.dkr.ecr.${local.region}.amazonaws.com/drive/api:${var.image_tag}"
      portMappings = [
        {
          containerPort = 8080
          protocol      = "tcp"
        }
      ]
      essential = true
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.ecs.name
          awslogs-region        = local.region
          awslogs-stream-prefix = "drive-api"
        }
      }
      environment = [
        {
          name  = "ASPNETCORE_ENVIRONMENT"
          value = "Development"
        }
      ]
    }
  ])

  tags = local.tags
}

# CloudWatch Log Group
resource "aws_cloudwatch_log_group" "ecs" {
  name              = "/ecs/drive-dev"
  retention_in_days = 7
  tags              = local.tags
}

# ECS Service
resource "aws_ecs_service" "drive_api" {
  name            = "drive-api"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.drive_api.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = data.aws_subnets.default.ids
    security_groups  = [aws_security_group.ecs_tasks.id]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.drive_api.arn
    container_name   = "drive-api"
    container_port   = 8080
  }

  depends_on = [aws_lb_listener.drive_api]

  tags = local.tags
}