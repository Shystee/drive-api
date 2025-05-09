module "ecr_ec1" {
  source = "../modules/ecr"

  repositories = toset([
    "drive/api",
  ])
  tags = local.tags
}