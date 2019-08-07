CREATE TABLE IF NOT EXISTS payments
(
	id VARCHAR(36) NOT NULL,
	merchant_id BIGINT REFERENCES merchants(id),
	merchant_name VARCHAR(100) NOT NULL,
	acquirer_id BIGINT REFERENCES acquirers(id),
	acquirer_name VARCHAR(100) NOT NULL,
	status VARCHAR(20) NOT NULL,
	response_code VARCHAR(5),
	amount INT NOT NULL,
	currency VARCHAR(5) NOT NULL,
	card_number VARCHAR NOT NULL,
	expiry_month VARCHAR(2) NOT NULL,
	expiry_year VARCHAR(4) NOT NULL,
	cardholder_name VARCHAR(100) NOT NULL,
	acquirer_payment_id VARCHAR(36),
	acquirer_status VARCHAR(50),
	acquirer_response_code VARCHAR(50),
	payment_time TIMESTAMPTZ NOT NULL,
	correlation_id VARCHAR NOT NULL,

	CONSTRAINT payments_pk PRIMARY KEY (id)
);

CREATE INDEX payments_request_id_index ON payments (correlation_id);