resource "aws_vpc" "this" {
  cidr_block           = var.vpc_cidr
  enable_dns_hostnames = true
  enable_dns_support   = true
  tags                 = var.tags
}

resource "aws_internet_gateway" "this" {
  vpc_id = aws_vpc.this.id
  tags   = var.tags
}

resource "aws_subnet" "public" {
  for_each = zipmap(var.availability_zones, var.public_subnet_cidrs)

  vpc_id                  = aws_vpc.this.id
  availability_zone       = each.key
  cidr_block              = each.value
  map_public_ip_on_launch = true
  tags                    = var.tags
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.this.id
  tags   = var.tags
}

resource "aws_route" "public_internet_access" {
  route_table_id         = aws_route_table.public.id
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = aws_internet_gateway.this.id
}

resource "aws_route_table_association" "public_assoc" {
  for_each       = aws_subnet.public
  subnet_id      = each.value.id
  route_table_id = aws_route_table.public.id
}

resource "aws_eip" "nat" {
  domain     = "vpc"
  depends_on = [aws_internet_gateway.this]
  tags       = var.tags
}

resource "aws_nat_gateway" "this" {
  allocation_id = aws_eip.nat[count.index].id
  subnet_id     = aws_subnet.public[0].id
  tags          = var.tags
  depends_on    = [aws_internet_gateway.this]
}

resource "aws_subnet" "private" {
  for_each = zipmap(var.availability_zones, var.private_subnet_cidrs)

  vpc_id            = aws_vpc.this.id
  availability_zone = each.key
  cidr_block        = each.value
  tags              = var.tags
}

resource "aws_route_table" "private" {
  vpc_id = aws_vpc.this.id
  tags   = var.tags
}

resource "aws_route" "private_nat_gateway" {
  route_table_id         = aws_route_table.private.id
  destination_cidr_block = "0.0.0.0/0"
  nat_gateway_id         = aws_nat_gateway.this.id
}

resource "aws_route_table_association" "private_assoc" {
  for_each       = aws_subnet.private
  subnet_id      = each.value.id
  route_table_id = aws_route_table.private.id
}
