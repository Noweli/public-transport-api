locals {
  containerName = "${local.region}_${local.product}_container"
}

resource "aws_ecs_task_definition" "ecs-task-definition" {
  family                = "service"
  container_definitions = jsonencode([
    {
      name         = local.containerName
      image        = aws_ecr_repository.ecr-repository.repository_url
      essential    = true
      portMappings = [
        {
          containerPort = 80
          hostPort      = 80
        }
      ],
      memory = 512,
      cpu    = 256
    }
  ])

  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  memory                   = 512
  cpu                      = 256
  execution_role_arn       = aws_iam_role.ecs-task_execution-role.arn
}


resource "aws_ecs_service" "ecs_service" {
  name            = "service"
  cluster         = aws_ecs_cluster.ecs-cluster.id
  task_definition = aws_ecs_task_definition.ecs-task-definition.arn
  launch_type     = "FARGATE"
  desired_count   = 1

  load_balancer {
    target_group_arn = aws_lb_target_group.target_group.arn
    container_name   = local.containerName
    container_port   = 80
  }

  network_configuration {
    subnets = [
      aws_default_subnet.default_subnet_a.id, aws_default_subnet.default_subnet_b.id,
      aws_default_subnet.default_subnet_c.id
    ]
    assign_public_ip = true
    security_groups = [aws_security_group.service_security_group.id]
  }
}

resource "aws_ecr_repository" "ecr-repository" {
  name = "${local.region}_${local.product}_ecr-repository"
}

resource "aws_ecs_cluster" "ecs-cluster" {
  name = "${local.region}_${local.product}_ecs-cluster"
}