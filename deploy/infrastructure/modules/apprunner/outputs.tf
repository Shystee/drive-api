output "service_url" {
  description = "The HTTP URL for your App Runner service"
  value       = aws_apprunner_service.this.service_url
}

output "service_arn" {
  description = "The ARN of the App Runner service"
  value       = aws_apprunner_service.this.arn
}

output "vpc_connector_arn" {
  description = "The ARN of the VPC Connector (if created)"
  value       = aws_apprunner_vpc_connector.this.arn
}
