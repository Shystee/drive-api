resource "aws_apprunner_vpc_connector" "this" {
  vpc_connector_name = "${var.service_name}-vpc-connector"
  subnets            = var.subnet_ids
  security_groups    = var.security_group_ids
  tags               = var.tags
}

resource "aws_apprunner_service" "this" {
  service_name = var.service_name

  source_configuration {
    authentication_configuration {
      access_role_arn = var.ecr_access_role_arn
    }
    image_repository {
      image_identifier      = "${var.image_repository}:${var.image_tag}"
      image_repository_type = "ECR"

      image_configuration {
        port                          = var.port
        runtime_environment_variables = var.environment_variables
      }
    }
  }

  instance_configuration {
    cpu               = var.cpu
    memory            = var.memory
    instance_role_arn = var.instance_role_arn
  }

  network_configuration {
    egress_configuration {
      egress_type       = "VPC"
      vpc_connector_arn = aws_apprunner_vpc_connector.this.arn
    }
  }

  tags = var.tags
}
