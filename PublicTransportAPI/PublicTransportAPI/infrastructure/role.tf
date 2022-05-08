data "aws_iam_policy_document" "assume_role_policy" {
  statement {
    actions = ["sts:AssumeRole"]

    principals {
      type        = "Service"
      identifiers = ["ecs-tasks.amazonaws.com"]
    }
  }
}

resource "aws_iam_role" "ecs-task_execution-role" {
  name               = "ecs-task_execution-role"
  assume_role_policy = data.aws_iam_policy_document.assume_role_policy.json
}


resource "aws_iam_role_policy_attachment" "ecs_role-policy-attachment" {
  role       = aws_iam_role.ecs-task_execution-role.name
  policy_arn = local.ecsTaskExecutionPolicyArn
}