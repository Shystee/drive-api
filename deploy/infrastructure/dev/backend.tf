terraform {
  backend "s3" {
    bucket = "drive-dev-terraform-state-933887400757"
    key    = "drive/dev/terraform.tfstate"
    region = "eu-central-1"
    use_lockfile = true
  }
}