output "vpc_id" {
  value = aws_vpc.this.id
}

output "public_subnet_ids" {
  value = [for s in aws_subnet.public : s.id]
}

output "private_subnet_ids" {
  value = [for s in aws_subnet.private : s.id]
}

output "internet_gateway_id" {
  value = aws_internet_gateway.this.id
}

output "nat_gateway_ids" {
  value = var.enable_nat_gateway ? [for ng in aws_nat_gateway.this : ng.id] : []
}
