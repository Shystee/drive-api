# Application Load Balancer
resource "aws_lb" "drive_api" {
  name               = "drive-dev-api"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb.id]
  subnets            = data.aws_subnets.default.ids

  enable_deletion_protection = false

  tags = local.tags
}

# Target Group
resource "aws_lb_target_group" "drive_api" {
  name        = "drive-dev-api"
  port        = 8080
  protocol    = "HTTP"
  vpc_id      = data.aws_vpc.default.id
  target_type = "ip"

  health_check {
    enabled             = true
    path                = "/healthz"
    healthy_threshold   = 2
    unhealthy_threshold = 2
    timeout             = 5
    interval            = 30
    matcher             = "200"
  }

  tags = local.tags
}

# Listener
resource "aws_lb_listener" "drive_api" {
  load_balancer_arn = aws_lb.drive_api.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.drive_api.arn
  }
}