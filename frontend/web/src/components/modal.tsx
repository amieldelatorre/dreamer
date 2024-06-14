import { PropsWithChildren, useEffect } from 'react';
import styles from '../styles/modal.module.css'
import { NestedMessage as NestedMessage } from '../types/types'
import { TertiaryTitle } from './text';


interface modalBaseProps {
  closeFunction: () => void
  visible: boolean
  id: string
}

export const ModalBase = (props: PropsWithChildren<modalBaseProps>) => {
  useEffect(() => {
    
    const close = (event: React.KeyboardEvent) => {
      if(event.key === "Escape"){
        props.closeFunction()
      }
    }

    document.addEventListener('keydown', (e: any) => close(e))
  return () => document.removeEventListener('keydown', (e: any) => close(e))
},[])
  return (<div id={props.id} className={`${styles.modalBase} ${props.visible ? styles.active : ''}`} onClick={props.closeFunction}>{props.children}</div>);
}


interface modalCloseButtonProps {
  closeFunction: () => void
}

export const ModalCloseButton = (props: PropsWithChildren<modalCloseButtonProps>) => {
  return (
    <button id="modal-close" className={styles.modalClosebutton} onClick={props.closeFunction}>
      <span>&times;</span>
    </button>
  );
}

interface modalContentProps {
  title: string
  extraClassNames: string[]
}

export const ModalContent = (props: PropsWithChildren<modalContentProps>) => {
  let classNames = [styles.modalContent, ...props.extraClassNames ].join(' ');

  const handleOnClick = (event: React.MouseEvent) => {
    event.stopPropagation();
  }

  return (
    <div className={classNames} onClick={handleOnClick}>
      <TertiaryTitle title={props.title}/>
      {props.children}
    </div>
  );
}

export const MessageList = (messageList: string[]) => {
  return (
    <ul>
      {messageList.map(message => <li key={message}>{message}</li>)}
    </ul>
  );
}


export const NestedMessageItems = (nestedMessage: NestedMessage) => {
  return (
    <section key={nestedMessage.name}>
      <p>{nestedMessage.name}</p>
      <ul>
        {nestedMessage.value.map(message => <li key={message}>{message}</li>)}
      </ul>
    </section>
  );
}


interface LoadingModalProps {
  closeFunction: () => void
  visible: boolean
  title: string
}

export const LoadingModal = (props: PropsWithChildren<LoadingModalProps>) => {
  return (
    <ModalBase id='loading-modal' closeFunction={props.closeFunction} visible={props.visible}>
      <ModalContent title={props.title} extraClassNames={[ styles.overflowHidden, styles.minHeight300, styles.genericModal ]}>
        <ModalCloseButton closeFunction={props.closeFunction}/>
        <div className={styles.loadingModalSpinner}></div>
      </ModalContent>
      
    </ModalBase>
  );
}

export enum ModalTypes {
  Error,
  Success,
  Generic
}


export interface ModalProps {
  type: ModalTypes
  visible: boolean
  title: string
  message?: string
  messages?: string[]
  nestedMessages?: NestedMessage[]
  closeFunction: () => any
  extraClassNames: string[]
}

export const DefaultModalProps = 
{
  type: ModalTypes.Generic,
  visible: false,
  title: '',
  closeFunction: function() {},
  extraClassNames: []
};

export const DefaultServerErrorModalProps = {
  type: ModalTypes.Error,
  visible: true,
  title: 'Error',
  nestedMessages: [{name: "Server", value: ["Cannot handle your request right now, please try again later"]}] as NestedMessage[]
}

export const Modal = (props: PropsWithChildren<ModalProps>) => {
  let currentModalStyle;
  switch (props.type) {
    case ModalTypes.Success:
      currentModalStyle = styles.successModal;
      break;
    case ModalTypes.Error:
      currentModalStyle = styles.errorModal;
      break;
    default:
      currentModalStyle = styles.genericModal;
      break;
  }
  
  return (
    <ModalBase id='default-modal' closeFunction={props.closeFunction} visible={props.visible}>
      <ModalContent title={props.title} extraClassNames={[...props.extraClassNames, styles.overflowYScroll, currentModalStyle ]}>
        <ModalCloseButton closeFunction={props.closeFunction}/>
        { props.message != null && <p>{props.message}</p> }
        { props.messages != null && MessageList(props.messages) }
        { props.nestedMessages != null && props.nestedMessages.map((nestedMessage) => NestedMessageItems(nestedMessage)) }
        { props.children }
      </ModalContent>

    </ModalBase>
  );
}
