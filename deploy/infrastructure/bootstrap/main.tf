# Configure the AWS Provider
provider "aws" {
  region = "eu-central-1" # Choose your desired AWS region
}

data "aws_caller_identity" "current" {}

# Create an S3 bucket to store the Terraform state
resource "aws_s3_bucket" "terraform_state" {
  bucket = "drive-dev-terraform-state-${data.aws_caller_identity.current.account_id}" # CHANGE THIS to a globally unique name

  # Prevent accidental deletion of the S3 bucket
  lifecycle {
    prevent_destroy = false
  }
}

# Enable versioning on the S3 bucket
resource "aws_s3_bucket_versioning" "terraform_state_versioning" {
  bucket = aws_s3_bucket.terraform_state.id
  versioning_configuration {
    status = "Enabled"
  }
}

# Enable server-side encryption by default for the S3 bucket
resource "aws_s3_bucket_server_side_encryption_configuration" "terraform_state_encryption" {
  bucket = aws_s3_bucket.terraform_state.id

  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

# Block all public access to the S3 bucket
resource "aws_s3_bucket_public_access_block" "terraform_state_public_access" {
  bucket = aws_s3_bucket.terraform_state.id

  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}
