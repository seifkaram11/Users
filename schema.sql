USE ecommerceuserdb;

CREATE TABLE "Users"(
    "UserID" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Email" VARCHAR NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "Name" TEXT,
    "Gender" VARCHAR,
    "RefreshToken" VARCHAR,
    "RefreshTokenExpiryTime" TIMESTAMPTZ
);
