# base
start-debug:
	docker-compose -f docker-compose-dev.yml up

stop:
	docker-compose stop
	
# with microservices
start:
	docker-compose -f docker-compose.yml up