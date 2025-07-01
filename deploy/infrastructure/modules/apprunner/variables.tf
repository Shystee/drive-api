variable "service_name" {
  description = "Name of the App Runner service"
  type        = string
}

variable "image_repository" {
  description = "ECR repository URI (without tag)"
  type        = string
}

variable "image_tag" {
  description = "Image tag to deploy (e.g. from gitversion)"
  type        = string
}

variable "ecr_access_role_arn" {
  description = "IAM Role ARN that allows App Runner to pull from ECR"
  type        = string
}

variable "subnet_ids" {
  description = "List of private subnet IDs for the VPC Connector"
  type        = list(string)
  default     = []
}

variable "security_group_ids" {
  description = "List of security group IDs for the VPC Connector"
  type        = list(string)
  default     = []
}

variable "cpu" {
  description = "CPU units for each instance"
  type        = string
  default     = "1024"
}

variable "memory" {
  description = "Memory (MB) for each instance"
  type        = string
  default     = "2048"
}

variable "port" {
  description = "Container listening port"
  type        = string
  default     = "80"
}

variable "tags" {
  description = "Tags to apply to all resources"
  type        = map(string)
  default     = {}
}

variable "instance_role_arn" {
  description = "IAM Role ARN for App Runner instance (runtime permissions)"
  type        = string
}

variable "environment_variables" {
  description = "Environment variables for the App Runner service"
  type        = map(string)
  default     = {}
}
