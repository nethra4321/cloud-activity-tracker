pipeline {
    agent any

    environment {
        DOCKERHUB_CREDENTIALS = credentials('dockerhub-credentials') // Jenkins credential ID
        AWS_CREDENTIALS = credentials('ec2-ssh') // Jenkins credential ID
        IMAGE_NAME = 'nethra4321/activity-backend'
        IMAGE_TAG = "latest"
        EC2_USER = 'ubuntu'
        EC2_HOST = '51.20.140.225'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'master', url: 'https://github.com/nethra4321/cloud-activity-tracker.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                sh 'docker build -t nethra4321/activity-backend:latest .'
            }
        }

        stage('Push to Docker Hub') {
            steps {
                sh """
                echo "$DOCKERHUB_CREDENTIALS_PSW" | docker login -u "$DOCKERHUB_CREDENTIALS_USR" --password-stdin
                docker push $IMAGE_NAME:$IMAGE_TAG
                """
            }
        }

        // stage('Upload Static to S3') {
        //     steps {
        //         sh """
        //         aws configure set aws_access_key_id $AWS_CREDENTIALS_USR
        //         aws configure set aws_secret_access_key $AWS_CREDENTIALS_PSW
        //         aws s3 sync ./frontend/dist s3://your-bucket-name --delete
        //         """
        //     }
        // }

        stage('Deploy on EC2') {
            steps {
                sh """
                ssh -o StrictHostKeyChecking=no $EC2_USER@$EC2_HOST '
                    export IMAGE_TAG=$IMAGE_TAG &&

                    docker-compose pull &&
                    docker-compose down &&
                    docker-compose up -d &&

                    docker exec -i redpanda rpk topic create activity-events || true &&

                    docker-compose restart activity-backend
                '
                """
            }
        }

    }

    post {
        success {
            echo "Deployment successful."
        }
        failure {
            echo "Deployment failed."
        }
    }
}
