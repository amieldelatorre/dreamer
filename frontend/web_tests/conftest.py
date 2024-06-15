import pytest
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.chrome.service import Service


def pytest_addoption(parser):
    parser.addoption(
        "--headless",
        action="store_true",
        default=False,
        help="Run headless"
    )

    parser.addoption(
        "--driver_path",
        action="store",
        default="./chromedriver/chromedriver-win64/chromedriver.exe",
        help="Path to the chrome driver"
    )


@pytest.fixture
def chrome_browser(pytestconfig):
    run_headless = pytestconfig.getoption("headless")
    driver_path = pytestconfig.getoption("driver_path")

    service = Service(executable_path=driver_path)

    if run_headless:
        chrome_options = Options()
        chrome_options.add_argument("--no-sandbox")
        chrome_options.add_argument("--disable-dev-shm-usage")
        chrome_options.add_argument("--headless")
        chrome_options.add_argument("--disable-gpu")
        driver = webdriver.Chrome(service=service, options=chrome_options)
    else:
        driver = webdriver.Chrome(service=service)

    driver.implicitly_wait(10)
    yield driver
    driver.quit()
