provider "aws" {
  region = "eu-west-1"
}

locals {
  region = "eu-west-1"
  product = "public-transport-api"
  ecsTaskExecutionPolicyArn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

