const Footer = () => {
    return (
      <div className='flex justify-between text-[12px] text-gray-500'>
       
        <span className="text-[12px] text-gray-500">
          COPYRIGHT © {new Date().getFullYear()}{' '}
          <a href='https://persol.net' className="text-blue-900" target='_blank' rel='noopener noreferrer'>
            Persol System Ltd.
          </a>
          <span >, All rights Reserved</span>
        </span>
        <span >
          Invoice verification App
       
        </span>
      </div>
    )
  }
  
  export default Footer