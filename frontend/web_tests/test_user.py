import time
from selenium.common import NoSuchElementException
from selenium.webdriver.common.by import By


def test_unsuccessful_signup_email_exists(chrome_browser):
    """  
    Test an unsuccessful signup where the email exists
    """
    url = "http://localhost:8079/signup"
    first_name = "Albert"
    last_name = "Einstein"
    email = "albert.einstein@example.invalid"
    password = "password"

    chrome_browser.get(url)
    first_name_box = chrome_browser.find_element(By.ID, value="firstName")
    first_name_box.send_keys(first_name)

    last_name_box = chrome_browser.find_element(By.ID, value="lastName")
    last_name_box.send_keys(last_name)

    email_box = chrome_browser.find_element(By.ID, value="email")
    email_box.send_keys(email)

    password_box = chrome_browser.find_element(By.ID, value="password")
    password_box.send_keys(password)

    password_confirm_box = chrome_browser.find_element(By.ID, value="confirmPassword")
    password_confirm_box.send_keys(password)

    submit_button = chrome_browser.find_element(By.ID, value="signup-submit")
    submit_button.click()

    time.sleep(2)

    try:
        modal = chrome_browser.find_element(By.ID, value="default-modal")
        assert modal.is_displayed(), "Error modal is not displayed"
    except NoSuchElementException:
        assert False, "Error modal could not be found"

    try:
        error_message = chrome_browser.find_element(By.XPATH, "//div[contains(@id,'default-modal')]/div/section/ul/li[1]")
        assert error_message.get_attribute("innerText") == "Email already exists"
    except NoSuchElementException:
        assert False, "Error message could not be found"

    try:
        error_button_close = chrome_browser.find_element(By.ID, value="error-modal-close")
        assert error_button_close.is_displayed(), "Error modal close could is not displayed"
    except NoSuchElementException:
        assert False, "Error modal close button could not be found"

    error_button_close.click()
    time.sleep(3)
    assert not modal.is_displayed(), "Modal is still displayed"


def test_successful_signup(chrome_browser):
    """
    Test an unsuccessful signup where the email exists
    """
    url = "http://localhost:8079/signup"
    first_name = "Stephen"
    last_name = "Hawking"
    email = "stephen.hawking@example.invalid"
    password = "password"

    chrome_browser.get(url)
    first_name_box = chrome_browser.find_element(By.ID, value="firstName")
    first_name_box.send_keys(first_name)

    last_name_box = chrome_browser.find_element(By.ID, value="lastName")
    last_name_box.send_keys(last_name)

    email_box = chrome_browser.find_element(By.ID, value="email")
    email_box.send_keys(email)

    password_box = chrome_browser.find_element(By.ID, value="password")
    password_box.send_keys(password)

    password_confirm_box = chrome_browser.find_element(By.ID, value="confirmPassword")
    password_confirm_box.send_keys(password)

    submit_button = chrome_browser.find_element(By.ID, value="signup-submit")
    submit_button.click()

    time.sleep(2)

    try:
        modal = chrome_browser.find_element(By.ID, value="default-modal")
        assert modal.is_displayed(), "Success modal is not displayed"
    except NoSuchElementException:
        assert False, "Success modal could not be found"

    try:
        modal_content_title = chrome_browser.find_element(By.XPATH, "//div[contains(@id,'default-modal')]/div/h3")
        assert modal_content_title.get_attribute("innerText") == "Created your account"
    except NoSuchElementException:
        assert False, "Modal title 'Created your account' could not be found"

    try:
        go_to_login_page_button = chrome_browser.find_element(By.ID, value="go-to-login-page-button")
        assert go_to_login_page_button.is_displayed(), "Go to login page button is not displayed"
    except NoSuchElementException:
        assert False, "Go to login page button could not be found"

    go_to_login_page_button.click()
    time.sleep(3)
    assert chrome_browser.current_url == "http://localhost:8079/login"
