# Basic Connect API

The Basic Connect API is a backend service designed to facilitate user authentication and basic account management for mobile applications. It provides simple and secure endpoints to enable user registration, login, logout, and related functionalities.

## Purpose

The primary purpose of the Basic Connect API is to empower mobile applications with essential user authentication features. Developers can seamlessly integrate this API into their apps, allowing users to create accounts, log in securely, and manage their basic account settings.

## Endpoints

Below are details for the available endpoints in the API.

### Login

- **Description:** Allows a previously registered user to log in and obtain an access token.
- **HTTP Method:** POST
- **Path:** `/v1/auth/login`
- **Request Body Parameters:**
  - `email`: The email address of the registered user.
  - `password`: The password associated with the email.
- **Request Body Parameters Constraints:**

  - `email`:
    - Format: Valid email format.
    - Length: Maximum 255 characters.
  - `password`:
    - Password should be provided as a SHA-256 hash.

- **Possible Responses:**

  - **Code:** 200 OK

    - **Description:** Successful login.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {
          "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI2OTU0ZWM4Ny1jMmYyLTQzOTUtOTYxZC1hZDI5NDg5YWQxMGEiLCJuYW1laWQiOiIxIiwibmJmIjoxNzAzNTc5NTM5LCJleHAiOjE3MDM4Mzg3MzksImlhdCI6MTcwMzU3OTUzOSwiaXNzIjoiYmFpY19jb25uZWN0X2FwaSIsImF1ZCI6ImJhaWNfY29ubmVjdF9hcHAifQ.FzRZCHGK4P19Zw_JybKsHK0qE29OtK5bVpsdMQS1J4E"
        }
      }
      ```

  - **Code:** 422 Unprocessable Entity

    - **Description:** The request was well-formed but was unable to be followed due to semantic errors.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "The 'email' field is not a valid e-mail address.",
        "data": {}
      }
      ```

  - **Code:** 401 Unauthorized

    - **Description:** Authentication failed.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "Invalid username or password",
        "data": {}
      }
      ```

  - **Code:** 500 Internal Server Error
    - **Description:** An unexpected server error occurred.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "An error has occurred",
        "data": {}
      }
      ```
