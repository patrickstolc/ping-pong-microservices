FROM fluent/fluentd:v1.15.1-1.0

# Use root to install plugins and dependencies
USER root

# Add build dependencies, install plugins, and remove build dependencies to keep the image small
RUN apk --no-cache --update add build-base ruby-dev
RUN gem uninstall -I elasticsearch
RUN gem install elasticsearch -v 7.10.1 \
  && gem install fluent-plugin-elasticsearch -v 5.0.3 --no-document \
  && gem install fluent-plugin-concat --no-document \
  && apk del build-base ruby-dev \
  && rm -rf /tmp/* /var/tmp/* /usr/lib/ruby/gems/*/cache/*.gem

# env
ENV FLUENTD_CONF="fluent.conf"

# Switch back to fluent user
USER fluent

ENV LD_PRELOAD=""
EXPOSE 24224 5140

USER fluent
ENTRYPOINT ["tini",  "--", "/bin/entrypoint.sh"]
CMD ["fluentd"]