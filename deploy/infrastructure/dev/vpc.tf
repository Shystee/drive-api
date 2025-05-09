# Use the default VPC for minimal setup
data "aws_vpc" "default" {
  default = true
}

# Get the default subnets
data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# Security group for the ALB
resource "aws_security_group" "alb" {
  name_prefix = "drive-dev-alb-"
  vpc_id      = data.aws_vpc.default.id
  description = "Security group for Drive API ALB"

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow HTTP traffic"
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow all outbound traffic"
  }

  tags = merge(local.tags, {
    Name = "drive-dev-alb-sg"
  })

  lifecycle {
    create_before_destroy = true
  }
}

# Security group for ECS tasks
resource "aws_security_group" "ecs_tasks" {
  name_prefix = "drive-dev-ecs-tasks-"
  vpc_id      = data.aws_vpc.default.id
  description = "Security group for Drive API ECS tasks"

  ingress {
    from_port       = 8080
    to_port         = 8080
    protocol        = "tcp"
    security_groups = [aws_security_group.alb.id]
    description     = "Allow traffic from ALB"
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow all outbound traffic"
  }

  tags = merge(local.tags, {
    Name = "drive-dev-ecs-tasks-sg"
  })

  lifecycle {
    create_before_destroy = true
  }
}