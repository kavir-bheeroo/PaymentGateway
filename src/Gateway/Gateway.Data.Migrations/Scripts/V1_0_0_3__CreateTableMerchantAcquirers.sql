CREATE TABLE IF NOT EXISTS merchant_acquirers
(
	id BIGSERIAL NOT NULL,
	merchant_id REFERENCES merchants(id),
	acquirer_id REFERENCES acquirers(id),

	CONSTRAINT merchants_pk PRIMARY KEY (id),
	CONSTRAINT merchant_acquirer_uq UNIQUE (merchant_id, acquirer_id)
);

CREATE INDEX merchant_id_index ON merchant_acquirers (merchant_id);