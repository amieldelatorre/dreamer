import { Link } from "react-router-dom"
import styles from '../styles/button.module.css';

interface LinkButtonProps {
  message: string
  link: string
  linkWrapperClassNames: string[]
  id: string
}

export const LinkButton = (props: LinkButtonProps) => {
  let linkClassNames = [props.linkWrapperClassNames].join(' ');

  return (
    <Link className={`${linkClassNames}`} to={props.link}>
      <button id={props.id} className={styles.linkButton}>
          {props.message}
      </button>
    </Link>
  );
}
