CREATE TABLE IF NOT EXISTS merchants
(
	id BIGSERIAL NOT NULL,
	name VARCHAR(100) NOT NULL,
	secret_key VARCHAR(36) NOT NULL,

	CONSTRAINT merchants_pk PRIMARY KEY (id),
	CONSTRAINT merchants_secret_key_uq UNIQUE (secret_key)
);