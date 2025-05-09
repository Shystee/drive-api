provider "aws" {
  region              = "eu-central-1"
  allowed_account_ids = [local.account_id]
  default_tags {
    tags = local.tags
  }
}