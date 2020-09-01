# CoEvent API

CoEvent is a flexible scheduling solution for teams and organizations.

The RESTful API provides endpoints to the datasources.

## Environment Variables

The following environment variables are required for the solution to work.
These are configured in the `.env` file.
The `.env` files are not included in source, so you may keep secrets in them.

| Key                                                   | Default Value                                | Note                                                    |
| ----------------------------------------------------- | -------------------------------------------- | ------------------------------------------------------- |
| ASPNETCORE_ENVIRONMENT                                | Development                                  | Controls the runtime environment settings               |
| ASPNETCORE_URLS                                       | https://_:443;http://_:80                    | The ports the API will listen to                        |
| ASPNETCORE_Kestrel**Certificates**Default\_\_Path     | /root/https/aspnetcore.pfx                   | Location of self-signed certificate for HTTPS           |
| ASPNETCORE_Kestrel**Certificates**Default\_\_Password |                                              | Password used when creating the self-signed certificate |
| DB_USERID                                             |                                              | The DB username                                         |
| DB_PASSWORD                                           |                                              | The DB password                                         |
| Authentication\_\_Issuer                              | https://localhost:10443/                     | JWT token issuer                                        |
| Authentication\_\_Audience                            | https://localhost:10443/                     | JWT token audience                                      |
| Authentication\_\_Salt                                |                                              | JWT token salt                                          |
| Authentication\_\_Secret                              |                                              | JWT token secret                                        |
| Cors\_\_WithOrigins                                   | http://localhost:3000 https://localhost:3000 | CORS configuration                                      |
| Mail\_\_Host                                          | smtp.ethereal.email                          | SMTP mail host                                          |
| Mail\_\_Port                                          | 587                                          | SMTP mail port                                          |
| Mail\_\_Name                                          | Jamey Pfeffer                                |                                                         |
| Mail\_\_Username                                      | jamey80@ethereal.email                       | SMTP mail username                                      |
| Mail\_\_Password                                      |                                              | SMTP mail password                                      |
| Mail\_\_FromEmail                                     | contact@victoriabiblestudy.com               | SMTP mail from address                                  |

## Development
