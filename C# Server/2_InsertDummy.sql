INSERT INTO users (username, password_hash) VALUES
('admin', 'admin'),
('utilizator2', 'hash_parola_2'),
('utilizator3', 'hash_parola_3');

INSERT INTO links (user_id, url, description) VALUES
(8, 'https://www.exemplu.com/produs1', 'Descriere produs 1'),
(9, 'https://www.exemplu.com/produs2', 'Descriere produs 2');

INSERT INTO links (user_id, url, description) VALUES
(1, 'https://www.exemplu.com/produs3', 'Descriere produs 3'),
(2, 'https://www.exemplu.com/produs4', 'Descriere produs 4');

INSERT INTO prices (link_id, price, currency) VALUES
(20, 100.50, 'RON'),
(20, 99.99, 'RON');

INSERT INTO prices (link_id, price, currency) VALUES
(21, 199.99, 'USD'),
(21, 189.99, 'USD');

INSERT INTO prices (link_id, price, currency) VALUES
(3, 49.99, 'EUR'),
(3, 45.99, 'EUR');



SELECT * FROM USERS;
SELECT * FROM links;
SELECT * FROM prices;
