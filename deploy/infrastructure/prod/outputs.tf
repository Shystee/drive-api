# VPC Outputs
output "vpc_id" {
  value = module.vpc.vpc_id
}

output "public_subnet_ids" {
  value = module.vpc.public_subnet_ids
}

output "private_subnet_ids" {
  value = module.vpc.private_subnet_ids
}

# RDS Outputs
output "db_endpoint" {
  value = module.rds.db_endpoint
}

output "db_port" {
  value = module.rds.db_port
}

# App Runner Outputs
output "app_service_url" {
  value = module.apprunner.service_url
}

output "app_service_arn" {
  value = module.apprunner.service_arn
}

# S3 CloudFront Outputs
output "s3_bucket_name" {
  value = module.s3_cloudfront.bucket_name
}

output "cloudfront_domain_name" {
  value = module.s3_cloudfront.cloudfront_domain_name
}

output "sqs_queue_url" {
  value = module.s3_cloudfront.sqs_queue_url
}
