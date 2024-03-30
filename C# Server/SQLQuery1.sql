CREATE TABLE USERS (
    Username NVARCHAR(100),
    Password NVARCHAR(100)
);

INSERT INTO Users (Username, Password) VALUES 
('utilizator1', 'parola1'),
('utilizator2', 'parola2');

SELECT * FROM Users;

DELETE FROM Users;



