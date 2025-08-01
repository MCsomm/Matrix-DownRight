﻿## Kubernetes Deployment

This app is deployed as a Kubernetes Job using Kind.

- Input matrix is passed via ConfigMap (`matrix-configmap.yaml`)
- App runs once, calculates the minimum path, writes down the used path, then exits
- Dockerized using `.NET 8` base images
- Kubernetes Job definition: `matrix-job.yaml`

### How to run it:

```bash
kind create cluster
docker build -t matrix-solver .
kubectl apply -f matrix-configmap.yaml
kubectl apply -f matrix-job.yaml
kubectl logs <pod-name>


