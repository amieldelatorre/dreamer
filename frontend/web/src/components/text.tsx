import styles from '../styles/text.module.css';

interface TitleProps {
  title: string
}


export const PrimaryTitle = (props: TitleProps) => {
  return (
    <h1 className={styles.primaryTitle}>{props.title}</h1>
  );
};

export const TertiaryTitle = (props: TitleProps) => {
  return (
    <h3 className={styles.tertiaryTitle}>{props.title}</h3>
  );
};
