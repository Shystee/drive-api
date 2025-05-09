output "load_balancer_dns" {
  description = "DNS name of the load balancer"
  value       = aws_lb.drive_api.dns_name
}

output "ecs_cluster_name" {
  description = "Name of the ECS cluster"
  value       = aws_ecs_cluster.main.name
}

output "ecs_service_name" {
  description = "Name of the ECS service"
  value       = aws_ecs_service.drive_api.name
}

output "ecr_repository_url" {
  description = "URL of the ECR repository for the API"
  value       = "${local.account_id}.dkr.ecr.${local.region}.amazonaws.com/drive/api"
}