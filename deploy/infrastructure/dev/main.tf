provider "aws" {
  region              = "eu-central-1"
  allowed_account_ids = ["933887400757"]
  default_tags {
    tags = local.tags
  }
}