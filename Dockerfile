FROM mono:4.2
RUN echo "deb http://download.mono-project.com/repo/debian wheezy-apache24-compat main" | tee -a /etc/apt/sources.list.d/mono-xamarin.list \
	&& echo "deb http://httpredir.debian.org/debian jessie main" > /etc/apt/sources.list \
	&& apt-get update -y \
	&& apt-get -y install mono-xsp4 \
		nuget \
		libapache2-mod-mono \
		apache2 \
		git \
	&& curl -o- https://raw.githubusercontent.com/creationix/nvm/v0.31.1/install.sh | bash \
	&& . ~/.bashrc \
	&& nvm install 5.5 \
	&& nvm use 5.5 \
	&& npm install -g bower
ADD . /app/
RUN cd app/ \
	&& nuget restore \
	&& xbuild /t:clean \
	&& xbuild \
	&& cd Ontwikkelopdracht \
	&& /bin/bash -c '. ~/.bashrc && bower --allow-root install' \
	&& cd .. \
	&& mv 100-mono.conf /etc/apache2/sites-available/ \
	&& a2ensite 100-mono \
	&& a2dissite 000-default \
	&& service apache2 stop \
	&& mv Ontwikkelopdracht/ /var/www/se2-ontwikkelopdracht \
	&& mkdir /etc/mono/registry \
	&& chmod a+rw /etc/mono/registry
EXPOSE 80
ENTRYPOINT ["/usr/sbin/apache2ctl", "-D", "FOREGROUND"]

