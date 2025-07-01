variable "environment" {
  description = "Environment name (dev, prod, etc.)"
  type        = string
}

variable "ecr_repository_arns" {
  description = "List of ECR repository ARNs that App Runner can access"
  type        = list(string)
  default     = ["*"] # Allows access to all ECR repos in the account
}

variable "secrets_manager_arns" {
  description = "List of Secrets Manager ARNs that App Runner can access"
  type        = list(string)
  default     = ["*"] # Allows access to all secrets
}

variable "s3_bucket_arns" {
  description = "List of S3 bucket ARNs that App Runner can access"
  type        = list(string)
  default     = []
}

variable "sqs_queue_arns" {
  description = "List of SQS queue ARNs that App Runner can access"
  type        = list(string)
  default     = []
}

variable "tags" {
  description = "Tags to apply to all resources"
  type        = map(string)
  default     = {}
}
