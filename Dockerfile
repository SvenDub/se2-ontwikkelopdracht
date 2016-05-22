FROM mono:4.2
RUN apt-get update -y \
	&& apt-get -y install mono-xsp4
ADD . /app/
WORKDIR /app/Ontwikkelopdracht
EXPOSE 5000
ENTRYPOINT ["/usr/bin/mono", "/usr/lib/mono/4.5/xsp4.exe", "--port=9000"]

