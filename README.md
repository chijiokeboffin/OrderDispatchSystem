# Dispatch Integration

## Architecture
- Receives JSON payload from D365 via Azure Functions.
- Transform the JSON to CSV using the mapping rules.
- Uploads CSV to 3PL SFTP using public key authentication.
- Implements resilience with retries and logging via Azure Application Insights.

## Deployment
1. Clone the repo
2. Configure Azure DevOps pipeline (see `azure-pipelines.yml`).
3. Deploy Azure Function and configure environment variables:
   - `SFTP_HOST`
   - `SFTP_USERNAME`
   - `SFTP_PRIVATE_KEY_PATH`

## Time Spent
- Design: 4 hours
- Implementation: 5 hours

## Future Improvements
- Implement persistence data store.
- Implement file integrity checks before SFTP upload.
- Add automated unit and integration testing.
- System testing, test end to end, this system has not been thoroughly tested because of time constraint.
- Create and configure the deployment pipline.
