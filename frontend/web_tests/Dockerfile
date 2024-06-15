FROM --platform=$BUILDPLATFORM python:3-bookworm
WORKDIR /usr/src/app

RUN apt update && apt upgrade -y
RUN apt install wget unzip -y
RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN apt install ./google-chrome-stable_current_amd64.deb -y && rm google-chrome-stable_current_amd64.deb 
RUN CHROME_VER=$(google-chrome --product-version) && \
    wget -q --continue -P ./chromedriver "https://storage.googleapis.com/chrome-for-testing-public/$CHROME_VER/linux64/chromedriver-linux64.zip"
RUN unzip ./chromedriver/chromedriver-linux64.zip -d ./chromedriver && rm ./chromedriver/chromedriver-linux64.zip
RUN ls ./chromedriver

COPY . .
RUN pip install -r requirements.txt

RUN useradd -ms /bin/bash selenium

CMD [ "pytest", "--headless", "--driver_path", "./chromedriver/chromedriver-linux64/chromedriver", "-v" ]
