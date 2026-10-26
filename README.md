# Azure CI/CD Reference Pipeline

A documentation-first example showing how a .NET application can be built, tested, and packaged using Azure DevOps. This is a starter reference; it does not deploy to a cloud account until you configure and authorize your own service connection.

## Pipeline stages
1. Restore dependencies
2. Build in Release mode
3. Run automated tests
4. Publish a deployable artifact

## Configure
1. Put your .NET solution and test project in the repository.
2. Review `azure-pipelines.yml` and change solution/test paths to match your project.
3. Create an Azure DevOps pipeline from the YAML file.
4. Add a deployment stage only after configuring an approved service connection and environment.

## Security checklist
- Use service connections with least privilege.
- Store secrets in secure pipeline variables or Key Vault; never in source control.
- Use approvals for production deployment.
- Keep build logs free of credentials and personal data.
- Pin and review dependencies regularly.

## Example workflow
`Pull request → Build → Test → Artifact → Approval → Deploy → Smoke test`

No live Azure resources are created by this repository.
