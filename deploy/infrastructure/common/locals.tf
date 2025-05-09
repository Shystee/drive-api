locals {

  environment_name = "common"
  account_id       = "933887400757"

  iam_path = "/${local.environment_name}/"

  tags = {
    "Environment" = local.environment_name
    "ManagedBy"   = "terraform"
  }
}

data "aws_region" "current" {}