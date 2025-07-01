terraform {
  required_version = ">= 1.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  backend "s3" {
    # Configure your backend here
    # bucket = "your-terraform-state-bucket"
    # key    = "drive-api/prod/terraform.tfstate"
    # region = "eu-central-1"
  }
}

provider "aws" {
  region = "eu-central-1"
}

locals {
  environment = "prod"
  common_tags = {
    Environment = local.environment
    Project     = "drive-api"
    ManagedBy   = "terraform"
  }
}

# Data sources for AZs
data "aws_availability_zones" "available" {
  state = "available"
}

# VPC Module
module "vpc" {
  source = "../modules/vpc"

  availability_zones   = slice(data.aws_availability_zones.available.names, 0, 3)
  public_subnet_cidrs  = ["10.1.1.0/24", "10.1.2.0/24", "10.1.3.0/24"]
  private_subnet_cidrs = ["10.1.11.0/24", "10.1.12.0/24", "10.1.13.0/24"]
  vpc_cidr             = "10.1.0.0/16"
  enable_nat_gateway   = true
  tags                 = local.common_tags
}

# RDS Module
module "rds" {
  source = "../modules/rds"

  vpc_id              = module.vpc.vpc_id
  private_subnet_ids  = module.vpc.private_subnet_ids
  db_name             = "drive_api_prod"
  db_username         = "postgres"
  publicly_accessible = false
  allowed_cidr_blocks = ["10.1.0.0/16"] # VPC CIDR
  tags                = local.common_tags
}

# S3 and CloudFront Module
module "s3_cloudfront" {
  source = "../modules/s3-cloudfront"

  environment            = local.environment
  bucket_name            = "drive-api-${local.environment}-files"
  sqs_queue_name         = "drive-api-${local.environment}-upload-completed"
  cloudfront_price_class = "PriceClass_All" # Better performance for prod
  tags                   = local.common_tags
}

# App Runner Module
module "apprunner" {
  source = "../modules/apprunner"

  service_name        = "drive-api-${local.environment}"
  image_repository    = "YOUR_ECR_REPOSITORY_URI" # Replace with your ECR URI
  image_tag           = "latest"
  ecr_access_role_arn = "YOUR_ECR_ACCESS_ROLE_ARN" # Replace with your existing role ARN
  subnet_ids          = module.vpc.private_subnet_ids
  security_group_ids  = [module.rds.security_group_id]
  cpu                 = "1024"
  memory              = "2048"
  port                = "3000"
  tags                = local.common_tags
}
