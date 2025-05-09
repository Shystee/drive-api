locals {

  environment_name = "dev"
  region           = data.aws_region.current.name
  account_id       = "933887400757"

  iam_path = "/${local.environment_name}/${local.region}/"


  tags = {
    "Environment" = local.environment_name
    "ManagedBy"   = "terraform"
  }
}

data "aws_region" "current" {}