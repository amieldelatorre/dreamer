import { ChangeEvent, FormEvent, useState } from 'react';
import { Link, NavigateFunction, useNavigate } from 'react-router-dom';
import { Modal, ModalTypes, ModalProps, DefaultModalProps, LoadingModal, HandleValidationModalErrors, HandleServerErrorModal, HandleNotOkErrorModal } from '../components/modal';
import { PrimaryTitle, TertiaryTitle } from '../components/text';
import { NestedMessage } from '../types/types';
import { Login, LoginCredentials, SetAccessToken } from '../api/dreamer_auth';
import { delay } from '../utils/delay';
import styles from '../styles/authform.module.css';
import textStyles from '../styles/text.module.css';
import modalStyles from '../styles/modal.module.css';


interface FormData {
  email: string
  password: string
}

const handleFormSubmit = async (
      event: FormEvent,
      formData: FormData,
      setModalArgs: React.Dispatch<React.SetStateAction<ModalProps>>,
      originalModalArgs: ModalProps,
      setModalVisible: React.Dispatch<React.SetStateAction<boolean>>,
      setIsLoading: React.Dispatch<React.SetStateAction<boolean>>,
      navigate: NavigateFunction
  ) => {
  event.preventDefault();
  let validationErrors = validateForm(formData.email, formData.password);
  if (validationErrors.length > 0) {
    HandleValidationModalErrors(setModalArgs, originalModalArgs, setModalVisible, validationErrors);
    return;
  }

  try {
    setIsLoading(true);
    let loginData: LoginCredentials = {
      email: formData.email,
      password: formData.password
    };
    let requestResult = await Login(loginData);

    if (requestResult == null || requestResult.status === 500) {
      HandleServerErrorModal(setModalArgs, originalModalArgs, setModalVisible);
      return;
    }
    else if (!requestResult.ok) {
      HandleNotOkErrorModal(requestResult, setModalArgs, originalModalArgs, setModalVisible);
      return;
    }

    else {
      setIsLoading(false);
      
      let jsonResponse = await requestResult.json();
      SetAccessToken(jsonResponse.Item["AccessToken"]);

      setModalArgs({
        ...originalModalArgs,
        type: ModalTypes.Success,
        visible: true,
        title: 'Created your account',
        message: "Redirecting you to the application",
        messages: undefined,
        nestedMessages: [],
        extraClassNames: [],
      })
      setModalVisible(true);
      await delay(1500);
      navigate("/");
      return;
    }

  } catch {
    HandleServerErrorModal(setModalArgs, originalModalArgs, setModalVisible);
    return;
  } finally {
    setIsLoading(false);
  }

}

const validateForm = (email: string, password: string) => {
  let errors: NestedMessage[] = [];
  
  if (email.trim().length === 0 || email === undefined || email === null)
    errors.push({name: "Email", value: ["Email must be in a valid format"]});
  if (password.trim().length === 0 || password === undefined || password === null) 
    errors.push({name: "Password", value: [`Cannot be null or empty `]});

  return errors;
}



const LoginPage = () => {
  const navigate = useNavigate();
  const [ modalVisible, setModalVisible ] = useState(false);
  let defaultModalProps = DefaultModalProps;
    const closeModal = () => {
        setModalVisible(false);
    }

  const [ modalArgs, setModalArgs ] = useState<ModalProps>({
      ...defaultModalProps,
      closeFunction: closeModal
  });

  const [ isLoading, setIsLoading ] = useState(false);
  const [ formData, setFormData ] = useState({
    email: "",
    password: "",
  });

  const handleChangeEvent = (event: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setFormData({
        ...formData,
        [name]: value
    });
  }

  return(
    <>
      <Modal 
          type={modalArgs.type} 
          visible={modalVisible} 
          title={modalArgs.title}
          closeFunction={modalArgs.closeFunction} 
          extraClassNames={modalArgs.extraClassNames}
          message={modalArgs.message}
          messages={modalArgs.messages}
          nestedMessages={modalArgs.nestedMessages}
          >
          {modalArgs.type == ModalTypes.Error && <button id="error-modal-close" className={modalStyles.errorCloseButton} onClick={closeModal}>Close</button>}
      </Modal>

      <LoadingModal 
          title="Logging you in..." 
          visible={isLoading} 
          closeFunction={() => setIsLoading(false)}
      />

      <div className={styles.pageBackground}>
          <form className={`${styles.form} ${styles.minHeight350} ${textStyles.textColorWhite}`} onSubmit={event => handleFormSubmit(event, formData, setModalArgs, modalArgs, setModalVisible, setIsLoading, navigate)}>
              <Link to='/' className={`${textStyles.textDecorationNone} ${textStyles.textColorWhite}`}><PrimaryTitle title="Dreamer"/></Link>
              <TertiaryTitle title="Login" />

              <input className={styles.input} type="email" name="email" id="email" minLength={1} placeholder='Email' onChange={handleChangeEvent}/>
              <input className={styles.input} type="password" name="password" id="password" minLength={1} placeholder='Password' onChange={handleChangeEvent}/>

              <div className={styles.submitDiv}>
                  <input id="login-submit" className={`${styles.submit} ${textStyles.textColorWhite}`} type='submit' />
                  <p className={textStyles.miniMessage}>Don't have an account? <Link to='/signup' className={`${textStyles.textColorGold}`}>Sign up</Link></p>
              </div>
          </form>
          
      </div>
    </>
  );
}

export default LoginPage;