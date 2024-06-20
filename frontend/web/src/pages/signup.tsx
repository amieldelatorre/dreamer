import { ChangeEvent, FormEvent, useState } from 'react';
import { Link } from 'react-router-dom';
import styles from '../styles/authform.module.css';
import textStyles from '../styles/text.module.css';
import modalStyles from '../styles/modal.module.css';
import { PrimaryTitle, TertiaryTitle } from '../components/text';
import { NestedMessage } from '../types/types';
import { Modal, ModalTypes, ModalProps, DefaultModalProps, LoadingModal, HandleValidationModalErrors, HandleServerErrorModal, HandleNotOkErrorModal } from '../components/modal';
import { LinkButton } from '../components/button';
import { CreateUser, CreateUserData } from '../api/dreamer_user';

interface FormData {
    firstName: string
    lastName: string
    email: string
    password: string
    confirmPassword: string
}


const handleFormSubmit = async (
        event: FormEvent, 
        setIsLoading: React.Dispatch<React.SetStateAction<boolean>>,
        formData: FormData,
        setModalVisible: React.Dispatch<React.SetStateAction<boolean>>,
        setModalArgs: React.Dispatch<React.SetStateAction<ModalProps>>,
        originalModalArgs: ModalProps
    ) => {
    event.preventDefault();
    
    let validationErrors = validateForm(formData.firstName, formData.lastName, formData.email, formData.password, formData.confirmPassword);
    
    if (validationErrors.length > 0) {
        HandleValidationModalErrors(setModalArgs, originalModalArgs, setModalVisible, validationErrors);
        return;
    }

    try {
        setIsLoading(true);
        let createUserData: CreateUserData = {
            firstName: formData.firstName,
            lastName: formData.lastName,
            email: formData.email,
            password: formData.password
        };
        let requestResult = await CreateUser(createUserData);

        if (requestResult == null || requestResult.status === 500) {
            HandleServerErrorModal(setModalArgs, originalModalArgs, setModalVisible);
            return;
        }
        else if (!requestResult.ok) {
            HandleNotOkErrorModal(requestResult, setModalArgs, originalModalArgs, setModalVisible);
            return;
        } 
        
        else {
            setModalArgs({
                ...originalModalArgs,
                type: ModalTypes.Success,
                visible: true,
                title: 'Created your account',
                message: undefined,
                messages: undefined,
                nestedMessages: [],
                extraClassNames: [],
            })
            setModalVisible(true);
        }
    } catch {
        HandleServerErrorModal(setModalArgs, originalModalArgs, setModalVisible);
        return;
    } finally {
        setIsLoading(false);
    }
};

const validateForm = (firstName: string, lastName: string, email: string, password: string, confirmPassword: string) => {
    const passwordMinimumLength = 8;
    let errors: NestedMessage[] = [];

    if (firstName.trim().length === 0 || firstName === undefined || firstName === null) 
        errors.push({name: "First Name", value: ["Cannot be null or empty"]})
    if (lastName.trim().length === 0 || lastName === undefined || lastName === null) 
        errors.push({name: "Last Name", value: ["Cannot be null or empty"]})
    if (email.trim().length === 0 || email === undefined || email === null)
        errors.push({name: "Email", value: ["Email must be in a valid format"]})
    if (password.trim().length < passwordMinimumLength || password === undefined || password === null) 
        errors.push({name: "Password", value: [`Password must have a minimum length of ${passwordMinimumLength}`]})
    if (password != confirmPassword)
        errors.push({name: "Password", value: [`Passwords don't match!`]})

    return errors;
}


const SignUpPage = () => {
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
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        confirmPassword: ""
    });

    const handleChangeEvent = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setFormData({
            ...formData,
            [name]: value
        });
    }

    return (
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
                {modalArgs.type == ModalTypes.Success && <LinkButton id="go-to-login-page-button" linkWrapperClassNames={[styles.minHeight40]} link='/login' message='Go to login page' />}
                {modalArgs.type == ModalTypes.Error && <button id="error-modal-close" className={modalStyles.errorCloseButton} onClick={closeModal}>Close</button>}
            </Modal>

            <LoadingModal 
                title="Creating your account..." 
                visible={isLoading} 
                closeFunction={() => setIsLoading(false)}
            />

            <div className={styles.pageBackground}>
                <form className={`${styles.form} ${styles.minHeight500} ${textStyles.textColorWhite}`} onSubmit={event => handleFormSubmit(event, setIsLoading, formData, setModalVisible, setModalArgs, modalArgs)}>
                    <Link to='/' className={`${textStyles.textDecorationNone} ${textStyles.textColorWhite}`}><PrimaryTitle title="Dreamer"/></Link>
                    <TertiaryTitle title="Sign Up" />

                    <input className={styles.input} type="text" name="firstName" id="firstName" minLength={1} placeholder='First Name' onChange={handleChangeEvent}/>
                    <input className={styles.input} type="text" name="lastName" id="lastName" minLength={1} placeholder='Last Name' onChange={handleChangeEvent}/>
                    <input className={styles.input} type="email" name="email" id="email" minLength={1} placeholder='Email' onChange={handleChangeEvent}/>
                    <input className={styles.input} type="password" name="password" id="password" minLength={8} placeholder='Password' onChange={handleChangeEvent}/>
                    <input className={styles.input} type="password" name="confirmPassword" id="confirmPassword" minLength={8} placeholder='Confirm Password' onChange={handleChangeEvent}/>

                    <div className={styles.submitDiv}>
                        <input id="signup-submit" className={`${styles.submit} ${textStyles.textColorWhite}`} type='submit' />
                        <p className={textStyles.miniMessage}>Already have an account? <Link to='/login' className={`${textStyles.textColorGold}`}>Log in</Link></p>
                    </div>
                </form>
                
            </div>
        </>
    );
}

export default SignUpPage;
