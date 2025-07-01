
variable "vpc_cidr" {
  description = "CIDR block for the VPC"
  type        = string
  default     = "10.0.0.0/16"
}

variable "availability_zones" {
  description = "List of AZs in which to create subnets"
  type        = list(string)
}

variable "public_subnet_cidrs" {
  description = "CIDRs for public subnets, one per AZ"
  type        = list(string)
}

variable "private_subnet_cidrs" {
  description = "CIDRs for private subnets, one per AZ"
  type        = list(string)
}

variable "enable_nat_gateway" {
  description = "Whether to provision a NAT Gateway for the private subnets"
  type        = bool
  default     = true
}

variable "tags" {
  description = "A map of tags to apply to all resources"
  type        = map(string)
  default     = {}
}
