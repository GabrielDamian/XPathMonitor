CREATE TABLE users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(50) UNIQUE NOT NULL,
    password_hash NVARCHAR(100) NOT NULL
);

CREATE TABLE links (
    link_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT REFERENCES users(user_id) ON DELETE CASCADE,
    url NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE prices (
    price_id INT PRIMARY KEY IDENTITY(1,1),
    link_id INT REFERENCES links(link_id) ON DELETE CASCADE,
    price DECIMAL(10, 2) NOT NULL,
    currency NVARCHAR(3) NOT NULL,
    date_added DATETIME2 DEFAULT GETDATE()
);

USE CCProject;









