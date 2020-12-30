$WORKSPACE = Split-Path $MyInvocation.MyCommand.Path -Parent

$BUILD_CONFIGURATION = "Release"
$BUILD_FRAMEWORK = "netcoreapp3.1"

$LAMBDA_PROFILE = 'default'
$LAMBDA_REGION = "ap-southeast-2"
$LAMBDA_S3_BUCKET = "dev-bindeng"
$LAMBDA_S3_PREFIX = "todo"
$LAMBDA_STACK_NAME = "SAM-TODO-Bindeng"
$LAMBDA_TEMPLATE = "$WORKSPACE/.aws/lambda/serverless.yml"

dotnet lambda deploy-serverless --profile               $LAMBDA_PROFILE `
                                --s3-bucket             $LAMBDA_S3_BUCKET `
                                --s3-prefix             $LAMBDA_S3_PREFIX `
                                --region                $LAMBDA_REGION `
                                --stack-name            $LAMBDA_STACK_NAME `
                                --template              $LAMBDA_TEMPLATE `
                                --configuration         $BUILD_CONFIGURATION `
                                --framework             $BUILD_FRAMEWORK
                                