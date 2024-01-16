# Basic Connect API

The Basic Connect API is a backend service designed to facilitate user authentication and basic account management for mobile applications. It provides simple and secure endpoints to enable user registration, login, logout, and related functionalities.

# Endpoints

Below are details for the available endpoints in the API.

## Login

- **Description:** Allows a previously registered user to log in and obtain an access token.
- **HTTP Method:** POST
- **Path:** `/v1/login`
- **Request Body Parameters:**
  - `email`: The email address of the registered user.
  - `password`: The password associated with the email.
- **Request Body Parameters Constraints:**

  - `email`:
    - Required: Yes.
    - Format: Should be a valid email address.
    - Length: Maximum 255 characters.
  - `password`:
    - Required: Yes.
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

  - **Code:** 400 Bad Request

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

## Logout

- **Description:** Allows a previously registered user to log out and revoke their token.
- **HTTP Method:** POST
- **Path:** `/v1/logout`
- **Request Headers:**

  - `Authorization: Bearer [JWT_TOKEN]`

- **Possible Responses:**

  - **Code:** 200 OK

    - **Description:** Successful logout.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {}
      }
      ```

  - **Code:** 401 Unauthorized

    - **Description:** Token is not valid.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "Invalid token. Please log in again.",
        "data": {}
      }
      ```

## Register

- **Description:** Register a new user.
- **HTTP Method:** POST
- **Path:** `/v1/user/register`
- **Request Body Parameters:**
  - `first_name`: First name of the user.
  - `last_name`: Last name of the user.
  - `email`: Email address of the user.
  - `password`: SHA-256 hash of the user's password.
- **Request Body Parameters Constraints:**

  - `first_name`:
    - Required: Yes.
    - Length: Maximum 100 characters.
  - `last_name`:
    - Required: Yes.
    - Length: Maximum 100 characters.
  - `email`:
    - Required: Yes.
    - Format: Should be a valid email address.
    - Length: Maximum 255 characters.
  - `password`:
    - Required: Yes.
    - Password should be provided as a SHA-256 hash.

- **Possible Responses:**

  - **Code:** 201 Created

    - **Description:** The user registration was successful.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {
          "user_id": 1
        }
      }
      ```

  - **Code:** 409 Conflict

    - **Description:** The provided email is already registered for another user.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "The email is already registered",
        "data": {}
      }
      ```

  - **Code:** 400 Bad Request

    - **Description:** The request couldn't be processed due to validation errors.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "The 'password' field must be a SHA-256 hash.",
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

## OTP Generation

### Endpoint: `/v1/otp/generate`

- **Description:** Generates a One-Time Password (OTP) and sends it to the user via the provided email address. The context indicates the reason for OTP generation, either to confirm the email or for password recovery.

- **HTTP Method:** `POST`

- **Path:** `/v1/otp/generate`

- **Request Body Parameters:**

  - `email`: The user's email address.
  - `context`: The context of OTP generation, which can be 'confirm_email' or 'password_recovery'.

- **Request Body Parameters Constraints:**

  - `email`:
    - Required: Yes.
    - Format: Should be a valid email address.
    - Length: Maximum 255 characters.
  - `context`:
    - Required: Yes.
    - Allowed values: 'confirm_email', 'password_recovery'.

- **Possible Responses:**
  - **Code:** 200 OK
    - **Description:** The OTP code was successfully generated and sent.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {}
      }
      ```
  - **Code:** 400 Bad Request
    - **Description:** The request could not be processed due to validation errors.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "The 'email' field is not a valid e-mail address.",
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

## OTP Validation

### Endpoint: `/v1/otp/validate`

- **Description:** Validates an OTP code entered by the user in the specified context.

- **HTTP Method:** `POST`

- **Path:** `/v1/otp/validate`

- **Request Body Parameters:**

  - `email`: The user's email address.
  - `otp_value`: The OTP code value entered by the user.
  - `context`: The context in which the OTP is being validated, which can be 'confirm_email' or 'password_recovery'.

- **Request Body Parameters Constraints:**

  - `email`:
    - Required: Yes.
    - Format: Should be a valid email address.
    - Length: Maximum 255 characters.
  - `otp_value`:
    - Required: Yes.
  - `context`:
    - Required: Yes.
    - Allowed values: 'confirm_email', 'password_recovery'.

- **Possible Responses:**
  - **Code:** 200 OK
    - **Description:** The OTP code validation was successful.
    - **Example response body (for 'password_recovery' context):**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {
          "temporary_access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJiMmFmYmRkZC02ODJkLTQ1YTctOGJhMi00NDEzNzgxZGQ3ZmIiLCJuYW1laWQiOiIxIiwibmJmIjoxNzA1MzMwODc3LCJleHAiOjE3MDUzMzE3NzcsImlhdCI6MTcwNTMzMDg3NywiaXNzIjoiYmFpY19jb25uZWN0X2FwaSIsImF1ZCI6ImJhaWNfY29ubmVjdF9hcHAifQ.QIp6LdP4xEyXmDRoWJLpPXPsbvW98rxnUk9T5yhl5TE"
        }
      }
      ```
    - **Example response body (for other contexts):**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {}
      }
      ```
  - **Code:** 401 Unauthorized
    - **Description:** OTP value is not valid.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "Invalid OTP value.",
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

## Password Reset

### Endpoint: `/v1/password/reset`

- **Description:** Resets the user's password.

- **HTTP Method:** `POST`

- **Path:** `/v1/password/reset`

- **Request Headers:**

  - `Authorization: Bearer [JWT_TOKEN]`

- **Request Body Parameters:**

  - `password`: The new password provided as a SHA-256 hash.

- **Request Body Parameters Constraints:**

  - `password`:
    - Required: Yes.
    - Password should be provided as a SHA-256 hash.

- **Possible Responses:**
  - **Code:** 200 OK
    - **Description:** The password reset was successful.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Password reset successful",
        "data": {}
      }
      ```
  - **Code:** 401 Unauthorized
    - **Description:** Token is not valid.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "Invalid token. Please log in again.",
        "data": {}
      }
      ```
  - **Code:** 422 Unprocessable Entity
    - **Description:** The request couldn't be processed due to validation errors.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "The 'password' field must be a SHA-256 hash.",
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

## Run Database Migrations

- **Description:** Run database migrations.
- **HTTP Method:** GET
- **Path:** `/v1/migration/run-migrations`

- **Possible Responses:**

  - **Code:** 200 OK

    - **Description:** The migrations was completed.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
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

## User By Id

- **Description:** Get user's information by id.
- **HTTP Method:** GET
- **Path:** `/v1/users/{id}`
- **Request Query Parameters:**

  - `id`: The user's id or 'me'.

- **Possible Responses:**

  - **Code:** 200 OK

    - **Description:** The migrations was completed.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {
          "first_name": "Nicolas Andres",
          "last_name": "Quintana Morales",
          "email": "nicolasquintanam@gmail.com"
        }
      }
      ```

  - **Code:** 403 Forbiden
    - **Description:** Doesn't have authorization.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "You are not authorized to access information for other users.",
        "data": {}
      }
      ```
