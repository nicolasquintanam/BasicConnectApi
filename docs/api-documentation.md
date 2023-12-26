# Basic Connect API

The Basic Connect API is a backend service designed to facilitate user authentication and basic account management for mobile applications. It provides simple and secure endpoints to enable user registration, login, logout, and related functionalities.

# Endpoints

Below are details for the available endpoints in the API.

## Login

- **Description:** Allows a previously registered user to log in and obtain an access token.
- **HTTP Method:** POST
- **Path:** `/v1/auth/login`
- **Request Body Parameters:**
  - `email`: The email address of the registered user.
  - `password`: The password associated with the email.
- **Request Body Parameters Constraints:**

  - `email`:
    - Required: Yes.
    - Format: Valid email format.
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

## Logout

- **Description:** Allows a previously registered user to log out and revoke their token.
- **HTTP Method:** POST
- **Path:** `/v1/auth/logout`

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
    - Format: Valid email format.
    - Length: Maximum 255 characters.
  - `password`:
    - Required: Yes.
    - Password should be provided as a SHA-256 hash.

- **Possible Responses:**

  - **Code:** 200 OK

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

## Send email to confirm

- **Description:** Send a email to a registered user to confirm their e-mail address.
- **HTTP Method:** POST
- **Path:** `/v1/emailconfirmation/send`
- **Request Body Parameters:**
  - `email`: Email address of the user to confirm.
- **Request Body Parameters Constraints:**

  - `email`:
    - Required: Yes.
    - Format: Valid email format.
    - Length: Maximum 255 characters.

- **Possible Responses:**

  - **Code:** 200 OK

    - **Description:** The email was sent successfully.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {}
      }
      ```

  - **Code:** 422 Unprocessable Entity

    - **Description:** The request couldn't be processed due to validation errors.
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

## Confirm email

- **Description:** Allows a registered user to confirm their e-mail address.
- **HTTP Method:** GET
- **Path:** `/v1/emailconfirmation/confirm`
- **Request URL Parameters:**

  - `email`: Email address of the user to confirm.
  - `token`: Confirmation token the user received to confirm the e-mail address.

- **Possible Responses:**

  - **Code:** 200 OK

    - **Description:** The e-mail address was confirmed successfully.
    - **Example response body:**
      ```json
      {
        "success": true,
        "message": "Operation completed successfully",
        "data": {}
      }
      ```

  - **Code:** 422 Unprocessable Entity

    - **Description:** The request couldn't be processed due to validation errors.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "The 'email' field is not a valid e-mail address.",
        "data": {}
      }
      ```

  - **Code:** 400 Bad Request

    - **Description:** Incorrect token or e-mail address.
    - **Example response body:**
      ```json
      {
        "success": false,
        "message": "An error has occurred",
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
