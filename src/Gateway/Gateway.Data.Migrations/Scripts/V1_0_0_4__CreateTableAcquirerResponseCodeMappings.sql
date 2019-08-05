CREATE TABLE IF NOT EXISTS acquirer_response_code_mappings
(
	id BIGSERIAL NOT NULL,
	acquirer_id BIGINT REFERENCES acquirers(id),
	acquirer_response_code VARCHAR(30) NOT NULL,
	gateway_response_code VARCHAR(5) NOT NULL, 

	CONSTRAINT acquirer_response_code_mappings_pk PRIMARY KEY (id)
);

CREATE INDEX acquirer_response_code_index ON acquirer_response_code_mappings (acquirer_response_code);